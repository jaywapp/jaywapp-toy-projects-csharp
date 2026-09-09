# 성능·안정성 실행 작업

orchestrator: Codex

| 작업 | owner | model | effort | depends_on | parallel_group | files | verification | status |
|---|---|---|---|---|---|---|---|---|
| 저장소 조사 | Codex | gpt-6-astra | high | 없음 | dotnet-repos | 규칙·README·소스 | 소스 검토 | completed |
| 확인된 개선 및 회귀 | Codex | gpt-6-astra | high | 저장소 조사 | dotnet-repos | tests/RegressionTests/Program.cs 및 관련 소스 | 아래 결과 | completed |
| 전체 기능 통합 검증 | Codex | gpt-6-astra | high | 확인된 개선 및 회귀 | dotnet-repos | 저장소 전체 | 아래 한계 | not_completed |

CliDetectionService에서 detector 한 개의 예외를 InvalidOrBroken 결과로 격리해 나머지 모델 점검을 계속한다. 취소는 전파하고 예외 유형만 Trace/진단에 남겨 민감 메시지 노출을 막는다.

검증: dotnet run --project tests/RegressionTests/RegressionTests.csproj: 7개 검사 통과.

한계: AIInstaller Core 실제 프로젝트 및 실패/권한 오류/취소/없는 실행파일 검사. 나머지 FMRookieScouter/WeddingVisitor/AICodeReviewRequester 앱 전체는 미검증.

위 완료 표시는 확인된 변경과 회귀 범위에 한정한다. 모든 기능·모든 실패 상황의 테스트 작성을 완료했다는 의미가 아니다. 그룹 간에는 상위 Codex 세션과 병렬 진행했고 그룹 내부는 조사→변경→검증 의존성으로 순차 진행했다. commit/push/배포 없음.
