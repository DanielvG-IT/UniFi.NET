.PHONY: help build test clean pack save release

SLN = UniFi.NET.slnx
CSPROJ = src/UniFi.Network.Client/UniFi.Network.Client.csproj

.DEFAULT_GOAL := help

help:
	@echo "🛠️  UniFi.NET Makefile Cheat Sheet"
	@echo ""
	@echo "Local Development:"
	@echo "  make build               Builds the .NET solution"
	@echo "  make test                Runs the unit tests"
	@echo "  make clean               Cleans bin/obj and artifacts"
	@echo "  make pack                Packs the NuGet package locally to ./artifacts"
	@echo ""
	@echo "Git & CI/CD Shortcuts:"
	@echo "  make save m=\"msg\"      Saves & pushes to main (Triggers Canary build matching .csproj version)"
	@echo "  make release             Tags & releases the current .csproj version to NuGet.org"

build:
	dotnet build $(SLN)

test:
	dotnet test $(SLN)

clean:
	dotnet clean $(SLN)
	rm -rf artifacts/

pack:
	dotnet pack $(CSPROJ) -c Release -o ./artifacts

save:
	@if [ -z "$(m)" ]; then \
		echo "❌ Error: Commit message not specified."; \
		echo "💡 Usage: make save m=\"Update devices endpoint\""; \
		exit 1; \
	fi
	git add src samples .github Makefile README.md Directory.Build.props *.slnx *.md 2>/dev/null || true
	git commit -m "$(m)"
	git push origin main
	@echo "✅ Pushed to main! Canary build started."

release:
	$(eval VERSION := $(shell sed -n 's/.*<Version>\(.*\)<\/Version>.*/\1/p' $(CSPROJ) | tr -d '\r'))
	@if [ -z "$(VERSION)" ]; then \
		echo "❌ Error: Could not find <Version> in $(CSPROJ)"; \
		exit 1; \
	fi
	@echo "🚀 Tagging release v$(VERSION) from .csproj..."
	git tag v$(VERSION)
	git push origin v$(VERSION)
	@echo "✅ Tag v$(VERSION) pushed! GitHub Actions is now publishing to NuGet.org."