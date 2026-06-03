# GitHub Actions Build and Release

This repository includes a release workflow at `.github/workflows/release.yml`.

## What it does

- Trigger on tag push (`v*`) and manual run (`workflow_dispatch`)
- Build the solution in `Release`
- Publish `AIInstaller.App` for `win-x64` as self-contained single-file
- Zip published output
- Create SHA-256 checksum file
- On tag push, create/update a GitHub Release and upload assets
- On manual run, upload build output as workflow artifact

## How to publish a new version

1. Commit and push your code to `main`.
2. Create and push a tag:

```powershell
git tag v0.1.0
git push origin v0.1.0
```

3. Open GitHub `Releases` page:
   - `https://github.com/jaywapp/AIInstaller/releases`
4. Users can download the latest build from:
   - `https://github.com/jaywapp/AIInstaller/releases/latest`

## Notes

- The workflow currently publishes `win-x64` only.
- If you need additional targets (for example `win-arm64`), add another publish step and include extra zip files in the release.
