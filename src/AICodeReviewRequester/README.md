# AICodeReviewRequester

Claude / Gemini API를 활용해 코드 리뷰를 요청하고 결과를 확인하는 WPF 데스크톱 애플리케이션.

## 기술 스택

| | |
|--|--|
| 언어 / 플랫폼 | C# / .NET 6.0 Windows |
| UI | WPF |
| MVVM | Prism + ReactiveUI |
| AI | Claude API, Gemini API |
| 직렬화 | Newtonsoft.Json |

## 구조

```
AICodeReviewRequester/
├── Models/AI/        # Claude, Gemini API 구현체
├── Interfaces/       # IAIModel, IAIResponse 추상화
├── Services/         # AIFactory (모델 생성 팩토리)
└── MainWindow        # UI + ViewModel
```

## 기능

- AI 모델 선택 (Claude / Gemini)
- 코드 입력 → 리뷰 요청 → 결과 표시
- IAIModel 인터페이스로 모델 확장 가능
