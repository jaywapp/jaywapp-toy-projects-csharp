# WeddingVisitor

Google Sheets와 연동하여 결혼식 하객 정보를 관리하는 WPF 데스크톱 애플리케이션.

## 기술 스택

| | |
|--|--|
| 언어 / 플랫폼 | C# / .NET 6.0 Windows |
| UI | WPF + WPF-UI |
| MVVM | Prism + ReactiveUI |
| 외부 연동 | Google Sheets API v4 (OAuth2) |

## 구조

```
WeddingVisitor/
├── Service/SheetManager  # Google Sheets 인증 및 데이터 읽기/쓰기
├── Model/Guest           # 하객 데이터 모델
├── Dialog/               # SelectHostDialog
└── Converter/            # Enum, Time 컨버터
```

## 기능

- Google Sheets OAuth2 인증
- 하객 목록 조회 / 추가 / 수정
- 하객 선택 다이얼로그
