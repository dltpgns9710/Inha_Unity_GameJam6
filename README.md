# Inha Unity GameJam6

| 항목 | 내용 |
|---|---|
| Engine | Unity 6000.4.8f1 |
| Render Pipeline | Universal 2D |
| Version Control | GitHub |

<br>

## 목차

- [1. 커밋 메시지 컨벤션](#1-커밋-메시지-컨벤션)
- [2. 기타 규칙](#2-기타-규칙)
- [3. 사용 에셋 크레딧](#3-사용-에셋-크레딧-asset-credits)
  - [3-1. Bloodlines UI](#3-1-bloodlines-ui-unity-asset-by-xgaida-shieldomirs)
  - [3-2. Character: Steven](#3-2-character-steven-free-by-kronovi-)
  - [3-3. Font (마루 부리)](#3-3-font-마루-부리-maruburi)
  - [3-4. Free Survival Horror Items Pack](#3-4-survival-horror-items-pack-by-leos-pixel)
  - [3-5. Horror Sound Effects](#3-5-horror-sound-effects-by-yourpalrob)
  - [3-6. House-tileset](#3-6-house-tileset-by-maschiat)
  - [3-7. Free Horror SFX](#3-7-free-horror-sfx---vn---scary-sound-effects-by-liminal-games)
  - [3-8. Melancholic Indie Horror Game Music Pack](#3-8-melancholic-indie-horror-game-music-pack-by-crow-shade)
  - [3-9. pH64 Pixel Pack](#3-9-ph64-pixel-pack---100s-of-sideview-assets--opengameartorg)
  - [3-10. PSX Horror Music & SFX Pack](#3-10-psx-horror-music--sfx-pack-by-pablo-alegria)
  - [3-11. Pet Dogs Pack](#3-11-pet-dogs-pack-by-luizmelo)
  - [3-12. Pixel Icons and Game controller](#3-12-pixel-icons-and-game-controller-2-by-disven)
  - [3-13. Overworld Objects](#3-13-overworld-objects--opengameartorg)
  - [3-14. Cursor Pixel Pack](#3-14-cursor-pixel-pack-by-kenney)

<br>

---

## 1. 커밋 메시지 컨벤션

### 1-1. 커밋 메시지 구조

```
태그: 요약문
- 작업자
- 상세 작업 내용 1 (선택 사항)
- 상세 작업 내용 2 (선택 사항)
```

<br>

### 1-2. 커밋 태그 종류

모든 태그는 **소문자**로 작성하고 콜론(`:`) 뒤에 한 칸을 띕니다.

| 태그 | 용도 | 예시 |
|---|---|---|
| `feat` | 새로운 기능 추가, 새로운 스크립트/에셋 생성 | `feat: 플레이어 이동 및 점프 기능 구현` |
| `fix` | 버그, 에러, 씬/프리팹 깨짐 현상 수정 | `fix: 셰이더 Y축 뒤집힘 및 암전 오류 수정` |
| `refactor` | 기능 변화 없이 코드 구조 개선, 변수명 변경, 구조 최적화 | `refactor: 웨이브 매니저 루프 구조 최적화` |
| `chore` | 코드 외적인 작업 (폴더 구조 생성, 패키지 설치, .gitignore/README 수정) | `chore: 프로젝트 README.md 코딩 컨벤션 추가` |

<br>

### 1-3. 작성 규칙

`~함`, `~했음` 보다는 `~구현`, `~수정`, `~제거` 등의 형태로 작성합니다.

```
권장: feat: 인벤토리 슬롯 드래그 앤 드롭 구현
지양: feat: 인벤토리 슬롯 드래그 앤 드롭 구현함

권장: fix: 점프 시 콜라이더 끼임 현상 제거
지양: fix: 점프 시 콜라이더 끼임 현상 제거했음
```

<br>

### 1-4. 커밋 메시지 예시

```
feat: 플레이어 이동 및 점프 기능 구현
- 홍길동
- Rigidbody 기반 이동 로직 작성
- 점프 시 이중 점프 방지 로직 추가
```

```
fix: 셰이더 Y축 뒤집힘 및 암전 오류 수정
- 김철수
```

<br>

---

## 2. 기타 규칙

- `main` 및 개발 브랜치 **직접 push 금지** — 반드시 별도 브랜치에서 작업 후 PR로 병합합니다.
- **pull 습관적으로** — 작업 시작 전 최신 변경 사항을 받아옵니다.
- **Base Scene 수정 금지** — 공용 씬은 임의로 수정하지 않습니다.

<br>

---

## 3. 사용 에셋 크레딧 (Asset Credits)

이 프로젝트에서 사용하는 외부 에셋과 라이선스 조건입니다. **빌드 시 아래 크레딧 표기를 반드시 포함해야 합니다.**

<br>

### 3-1. [BloodLines UI (Unity Asset) by xGaida, Shieldomirs](https://xgaida.itch.io/bloodlines-ui)

| 항목 | 내용 |
|---|---|
| 제작자 | xGaida, Shieldomirs |
| 출처 | itch.io |
| 라이선스 | 무료 (커스텀 라이선스) |

**조건**
- 상업적/비상업적 프로젝트 모두 사용 가능
- 무제한 게임에 사용 가능
- 에셋 자체의 재판매 및 재패키징 금지
- 크레딧 표기 의무는 없음

<br>

### 3-2. [Character: Steven [FREE] by Kronovi-](https://darkpixel-kronovi.itch.io/character-steven-free)

| 항목 | 내용 |
|---|---|
| 제작자 | Kronovi |
| 출처 | itch.io |
| 라이선스 | 무료 (커스텀 라이선스) |

**조건**
- 에셋 자체의 재판매 및 재배포 금지
- 수정 가능
- 비상업적/상업적 프로젝트 모두 사용 가능
- 상업적으로 사용할 경우 제작자에게 기부를 권장함 (필수는 아님)

<br>

### 3-3. Font (마루 부리, MaruBuri)

- [눈누 폰트 페이지](https://noonnu.cc/font_page/487)
- [네이버 한글한글 아름답게 (다운로드)](https://hangeul.naver.com/fonts/search?f=maru)

| 항목 | 내용 |
|---|---|
| 제작자 | 네이버 (네이버문화재단) |
| 출처 | 네이버 한글한글 아름답게 |
| 라이선스 | 네이버 나눔글꼴 라이선스 (OFL 기반, 상업용 무료) |

**조건**
- 개인/기업 모두 무료 사용, 인쇄물/웹사이트/영상/BI·CI 등 사용 가능
- 수정 및 재배포 가능
- 폰트 파일 자체를 유료로 판매하는 것은 금지
- 라이선스 전문을 포함하기 어려울 경우 출처 표기를 권장함

<br>

### 3-4. [Survival Horror Items Pack by Leo's Pixel](https://leos-pixel.itch.io/survival-horror-items-pack)

| 항목 | 내용 |
|---|---|
| 제작자 | Leo's Pixel |
| 출처 | itch.io |
| 라이선스 | 무료 (커스텀 라이선스) |

**조건**
- 상업적/비상업적 프로젝트 모두 사용 가능
- 수정 가능
- 수정 여부와 관계없이 재배포 및 재판매 금지
- 크레딧 표기 의무는 없음

<br>

### 3-5. [Horror Sound Effects by YourPalRob](https://yourpalrob.itch.io/must-have-horror-sound-effects)

| 항목 | 내용 |
|---|---|
| 제작자 | YourPalRob |
| 출처 | itch.io |
| 라이선스 | 무료 (커스텀 라이선스, 무료/유료 번들 혼합) |

**조건**
- 개인/상업 프로젝트 모두 크레딧 표기 없이 사용 가능
- 게임, 영상, 음악 등 미디어 프로젝트에 자유롭게 사용 가능
- 사운드 자체를 단독 파일로 재판매·재배포·리패키징 금지
- 크레딧 표기 의무는 없음

<br>

### 3-6. [House-tileset by maschiaT](https://maschiat.itch.io/house-tileset)

| 항목 | 내용 |
|---|---|
| 제작자 | maschiaT |
| 출처 | itch.io |
| 라이선스 | 무료 (커스텀 라이선스) |

**조건**
- 모든 종류의 프로젝트에 무료 사용 가능
- 수정 가능
- 크레딧 표기 필수 (maschiaT@wememo.art)

<br>

### 3-7. [FREE HORROR SFX - VN - SCARY SOUND EFFECTS by Liminal Games](https://liminal-space-dev.itch.io/free-horror-sfx-sounds)

| 항목 | 내용 |
|---|---|
| 제작자 | Liminal Games |
| 출처 | itch.io |
| 라이선스 | **CC0** (Creative Commons Zero) |

**조건**
- 상업적/비상업적 프로젝트 모두 사용 가능, 수정 가능
- 크레딧 표기 불필요
- 팩 자체의 재배포 및 재판매 금지
- AI로 생성된 사운드가 포함되어 있음 (AI-generated)

<br>

### 3-8. [Melancholic Indie Horror Game Music Pack by Crow Shade](https://crowshade.itch.io/melancholic-indie-horror-game-soundtrack-pack)

| 항목 | 내용 |
|---|---|
| 제작자 | Crow Shade |
| 출처 | itch.io |
| 라이선스 | **CC-BY 4.0** |

**조건**
- 무료/상업적 프로젝트 모두 사용 가능
- 크레딧 표기 필수

<br>

### 3-9. [pH64 Pixel Pack - 100s of Sideview Assets | OpenGameArt.org](https://opengameart.org/content/ph64-pixel-pack-100s-of-sideview-assets)

| 항목 | 내용 |
|---|---|
| 제작자 | PurpleHeart (Stacy Kendra Love) / 브랜드명: Narcissist Interactive |
| 출처 | OpenGameArt.org |
| 라이선스 | CC-BY 4.0 / CC-BY 3.0 / GPL 3.0 / GPL 2.0 / OGA-BY 3.0 중 선택 가능 |

**조건**
- 5개 라이선스 중 하나를 선택해서 사용 가능 (가장 쓰기 쉬운 **CC-BY 4.0** 권장)
- 작가 **이름(Stacy Kendra Love)** 과 **브랜드명(Narcissist Interactive)** 모두 크레딧 표기 필수
- 비디오/게임 형태로 출시 시 두 이름 모두 표기 필요
- 온라인 갤러리(Instagram, OpenGameArt, deviantArt, 개인 블로그 등)에 게시 시 리소스 링크 포함 필요

<br>

### 3-10. [PSX Horror Music & SFX Pack by Pablo Alegria](https://pabloalegria9.itch.io/psxhorrorpack)

| 항목 | 내용 |
|---|---|
| 제작자 | Pablo Alegria |
| 출처 | itch.io |
| 라이선스 | Royalty-Free License |

**조건**
- 상업적/비상업적 프로젝트 모두 로열티 없이 사용 가능
- 크레딧 표기 의무는 명시되어 있지 않음

<br>

### 3-11. [Pet Dogs Pack by LuizMelo](https://luizmelo.itch.io/pet-dogs-pack)

| 항목 | 내용 |
|---|---|
| 제작자 | LuizMelo |
| 출처 | itch.io |
| 라이선스 | **CC0** (Creative Commons Zero) |

**조건**
- 자유롭게 상업적/비상업적 사용 가능
- 크레딧 표기 불필요 (단, 제작자는 크레딧 표기를 권장함)

<br>

### 3-12. [Pixel Icons and Game Controller 2 by Disven](https://disven.itch.io/pixel-icons-and-game-controller-2)

| 항목 | 내용 |
|---|---|
| 제작자 | Disven |
| 출처 | itch.io |
| 라이선스 | **CC-BY 4.0** |

**조건**
- 상업적 사용 가능, 수정 가능
- 크레딧 표기는 필수는 아니라고 명시되어 있으나, 라이선스가 CC-BY 4.0이므로 크레딧 표기를 권장함

<br>

### 3-13. [Overworld Objects | OpenGameArt.org](https://opengameart.org/content/overworld-objects)

| 항목 | 내용 |
|---|---|
| 제작자 | Kelvin Shadewing |
| 출처 | OpenGameArt.org |
| 라이선스 | **CC-BY-SA 4.0** 또는 **GPL 3.0** 중 선택 |

**조건 (CC-BY-SA 4.0 선택 시)**
- 상업적 사용 가능, 수정 가능
- 출처 표기 필수 — **kelvinshadewing.net 링크 포함 필수**
- ShareAlike — 이 에셋을 수정해서 만든 2차 저작물은 동일한 라이선스(CC-BY-SA)로 배포해야 함

<br>

### 3-14. [Cursor Pixel Pack by Kenney](https://kenney.nl/assets/cursor-pixel-pack)

| 항목 | 내용 |
|---|---|
| 제작자 | Kenney |
| 출처 | kenney.nl |
| 라이선스 | **CC0** (Creative Commons Zero) |

**조건**
- 상업적/비상업적 프로젝트 모두 사용 가능, 수정 가능
- 크레딧 표기 불필요

<br>

### 3-15. 빌드 시 크레딧 표기 위치

위 크레딧 문구는 게임 내 다음 위치 중 한 곳에 반드시 포함합니다.

- 게임 시작 화면 또는 메인 메뉴의 "Credits" 항목
- 엔딩 크레딧 롤
- 게임 설명서(설치 파일 동봉 README, 스토어 페이지 설명 등)

> 에셋 추가/교체 시 이 섹션도 함께 업데이트해야 합니다.
