# 규칙 파일 관리

## 개요

AIInstaller는 RULE.md를 단일 소스로 사용하여 여러 AI CLI 도구의 규칙을 통합 관리합니다.

```
RULE.md          ← 사용자가 편집하는 유일한 파일
    │
    ├──▶ CLAUDE.md    ← "RULE.md를 읽으세요" 참조 자동 삽입
    └──▶ AGENTS.md    ← "RULE.md를 읽으세요" 참조 자동 삽입
```

## 파일 구조

### RULE.md

모든 AI CLI에 공통 적용되는 규칙을 담는 마크다운 파일입니다. 마법사의 Step 3에서 인라인 에디터로 편집할 수 있습니다.

### CLAUDE.md

Claude Code가 읽는 설정 파일입니다. RULE.md 저장 시 다음 참조 지시문이 자동으로 추가됩니다:

```markdown
# Claude Code Configuration

이 파일과 동일한 디렉토리에 있는 RULE.md 파일의 모든 규칙을 반드시 읽고 준수하세요.
```

### AGENTS.md

Codex가 읽는 설정 파일입니다. CLAUDE.md와 동일한 방식으로 참조가 추가됩니다:

```markdown
# Codex Agent Configuration

이 파일과 동일한 디렉토리에 있는 RULE.md 파일의 모든 규칙을 반드시 읽고 준수하세요.
```

## 자동 참조 삽입 규칙

`GlobalRuleFileManager`가 CLAUDE.md/AGENTS.md를 처리할 때:

| 상황 | 동작 |
|------|------|
| 파일이 없는 경우 | 제목 + 참조 지시문으로 새 파일 생성 |
| 파일이 있고 "RULE.md" 참조가 없는 경우 | 기존 내용 앞에 참조 지시문을 추가 |
| 파일이 있고 "RULE.md" 참조가 있는 경우 | 변경 없음 (중복 방지) |

기존 파일의 내용은 항상 보존됩니다.

## RuleSet 저장소

마법사가 내부적으로 사용하는 규칙셋 저장소입니다.

### 저장 경로

```
%APPDATA%/AIInstaller/rulesets/
├── default.ruleset.json       # 기본 규칙셋
├── custom.ruleset.json        # 사용자 정의 규칙셋
└── model-mapping.json         # 모델-규칙셋 매핑
```

### default.ruleset.json 예시

```json
{
  "Name": "default",
  "Version": "1.0.0",
  "Rules": [
    "Do not ask follow-up questions unless blocked.",
    "Allow access to approved workspace directories.",
    "Require completion report after each task.",
    "Enforce WPF and MVVM development conventions."
  ]
}
```

### model-mapping.json 예시

```json
[
  { "ModelIdentifier": "Codex", "RuleSetFileName": "default.ruleset.json" },
  { "ModelIdentifier": "ClaudeCode", "RuleSetFileName": "default.ruleset.json" }
]
```

## 디렉토리 자동 탐지

마법사 시작 시 앱 실행 경로에서 상위 디렉토리를 순회하며 다음 파일 중 하나라도 존재하는 디렉토리를 찾습니다:

- `CLAUDE.md`
- `AGENTS.md`
- `RULE.md`

찾지 못하면 `%USERPROFILE%`(사용자 홈 디렉토리)을 기본 경로로 사용합니다. Step 3의 "대상 디렉토리" 텍스트 박스에서 수동으로 변경할 수도 있습니다.
