# Contributing to UniFi.NET

First off, thank you for considering contributing to UniFi.NET! It's people like you that make open-source software great.

## Where do I start?

* **Bug Reports:** If you find a bug, please open an issue describing the problem, how to reproduce it, and what you expected to happen.
* **Feature Requests:** If you'd like a new feature or support for a new UniFi API endpoint, please open an issue first to discuss it before you start writing code.
* **Pull Requests:** If you're ready to submit code, please make sure you've discussed the changes in an issue first (unless it's a simple typo or small fix).

## Local Development

We use a standard `.NET` toolchain. To make local development easy, we've included a `Makefile`.

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download) (Check `global.json` or the `.csproj` for the required version)
* Make (Available by default on macOS and Linux. Windows users can use WSL or run `dotnet` commands manually)

### Using the Makefile

You can run `make help` to see all available commands. The most common ones are:

* `make build` - Restores dependencies and builds the solution.
* `make test` - Runs all unit tests.
* `make clean` - Cleans up the `bin/`, `obj/`, and `artifacts/` directories.

## Pull Request Process

1. Fork the repository and create your branch from `main`.
2. Ensure your code follows standard C# naming conventions and styling.
3. If you've added code that should be tested, add tests.
4. Run `make build` and `make test` locally to ensure everything works.
5. Update the `README.md` if your changes require new documentation.
6. Open a Pull Request!

Once your PR is merged into `main`, GitHub Actions will automatically generate a `-canary` package for testing. Official releases are cut via tags.