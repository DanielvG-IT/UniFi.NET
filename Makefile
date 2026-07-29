.PHONY: help build test clean pack save release

SLN = UniFi.NET.slnx
PROJECTS ?= $(wildcard src/*/*.csproj)
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
	@echo "  make release             Tags each package version from src/*/*.csproj for NuGet.org"
	@echo "  make release PROJECT=src/UniFi.Network.Client/UniFi.Network.Client.csproj"
	@echo "  make release DRY_RUN=1   Preview the tags without creating them"

build:
	dotnet build $(SLN)

test:
	dotnet test $(SLN)

clean:
	dotnet clean $(SLN)
	rm -rf artifacts/

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
	@set -e; \
	projects="$(if $(PROJECT),$(PROJECT),$(PROJECTS))"; \
	for csproj in $$projects; do \
		if [ ! -f "$$csproj" ]; then \
			echo "❌ Error: Project not found: $$csproj"; \
			exit 1; \
		fi; \
		version=$$(sed -n 's/.*<Version>\(.*\)<\/Version>.*/\1/p' "$$csproj" | tr -d '\r' | head -n 1); \
		if [ -z "$$version" ]; then \
			echo "❌ Error: Could not find <Version> in $$csproj"; \
			exit 1; \
		fi; \
		pkg=$$(basename "$$csproj" .csproj); \
		tag="$$pkg-v$$version"; \
		if git rev-parse -q --verify "refs/tags/$$tag" >/dev/null 2>&1; then \
			echo "⚠️  Tag $$tag already exists; skipping."; \
		elif [ "$(DRY_RUN)" = "1" ]; then \
			echo "🔎 [dry-run] Would tag $$tag from $$csproj"; \
		else \
			echo "🚀 Tagging release $$tag from $$csproj..."; \
			git tag "$$tag"; \
			git push origin "$$tag"; \
			echo "✅ Tag $$tag pushed! GitHub Actions is now publishing the package(s) from that tag."; \
		fi; \
	done
