# 📦 PwaHub (XpHub + XpVoice 통합 클라이언트 PWA)

아파트 단지용 백오피스 시스템인 **XpHub**와 음성 방송 시스템 **XpVoice**를  
통합하여 웹브라우저에서 사용 가능한 **Progressive Web App (PWA)** 으로 구현한 클라이언트입니다.  
관리자는 브라우저 또는 모바일에서 바로 방송, 공지, 모듈 관리가 가능합니다.


 기술 스택

| 영역       | 기술 |
|------------|------|
| Frontend   | Blazor WebAssembly (.NET 6) |
| UI 라이브러리 | Radzen Blazor Components |
| 상태 관리  | SignalR, LocalStorage |
| 인증       | JWT 기반 토큰 인증 |
| 서버       | .NET Web API (IIS 배포 환경) |
| 기타       | PWA 기능 (오프라인 실행, 설치 지원) |



  주요 기능

  Voice (XpVoice)
- 음성 방송 예약/실행
- 실시간 방송 제어
- 음성 파일 업로드 및 변환
- 방송 로그 및 이력 조회

 Hub (XpHub)
- 로그인 및 인증 처리
- 모듈 통합 진입점 관리
- 시스템 설정/버전 정보 조회
- API Gateway 스타일 요청 중계

---

 UI 예시 (향후 추가 가능)

> 클론한 사용자도 실행만으로 전체 동작 확인 가능  
(민감 정보는 `.gitignore` 처리됨)

---

 실행 방법

1. `git clone` 후 Visual Studio 2022 또는 CLI로 열기
2. 민감정보 파일 `appsettings.json`은 개인적으로 구성 필요
3. `Client`, `Shared`, `Server` 구조로 구성되어 있음


dotnet run --project Client  # Blazor 실행


 프로젝트 구조

Hub/
├── Client/     # Blazor WASM 클라이언트
├── Server/     # Web API (내부용, 현재 레포엔 미포함)
├── Shared/     # 공용 DTO/Model 정의
└── README.md   # 설명 파일
🚧 향후 계획
CI/CD 파이프라인 (GitHub Actions)

appsettings.json → appsettings.Example.json 제공

유닛 테스트 + E2E 테스트 추가

모바일 뷰 최적화 (Responsive 개선)

 만든 사람
Byekim 
