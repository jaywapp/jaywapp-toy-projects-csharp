# Troubleshooting

## npm 또는 Node.js가 없음

증상:
- Step 2에서 설치 실패
- `node --version` 또는 `npm --version` 실행 실패

해결:
1. Node.js LTS를 설치합니다.
2. 새 터미널에서 `node --version`, `npm --version`을 확인합니다.
3. AIInstaller를 다시 실행합니다.

## 설치는 됐는데 실행 확인이 실패함

증상:
- 설치 직후에도 `미설치` 또는 `설치 손상 또는 실행 실패`로 표시

원인:
- 현재 프로세스 PATH에 글로벌 npm 경로가 아직 반영되지 않았을 수 있습니다.

해결:
1. 새 터미널에서 `where claude` 또는 `where codex`를 확인합니다.
2. 경로가 나오면 앱을 다시 실행합니다.
3. 경로가 없으면 CLI 설치를 다시 시도합니다.

## 로그인 연결이 안 됨

증상:
- 상태가 `설치됨, 로그인 필요`로 유지됨

해결:
- Claude Code: 열린 창에서 `/login`을 실행하거나 `ANTHROPIC_API_KEY`를 설정합니다.
- Codex: `codex --login`을 완료하거나 `OPENAI_API_KEY`를 설정합니다.

## 규칙 파일 저장 경로가 의도와 다름

증상:
- Step 3의 기본 경로가 원하는 프로젝트가 아님

해결:
1. Step 3의 저장 디렉터리를 직접 수정합니다.
2. 저장 후 해당 폴더에 `RULE.md`, `CLAUDE.md`, `AGENTS.md`가 생겼는지 확인합니다.

## 빌드 확인

```powershell
dotnet build AIInstaller.sln
```
