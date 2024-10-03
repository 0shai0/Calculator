# Project Name : Calculator
# 계산기 프로그램 개발

### 팀원
0shai0 (1인 개발)

## 프로젝트 소개   
기본적인 계산기 기능을 구현하면서 코드의 가독성과 예외 처리에 주의를 기울였습니다.

#### 상세기능
1. 기본 연산 기능
    - 덧셈, 뺄셈, 곱셈, 나눗셈 등의 기본 연산 지원.
    - 각 연산은 단일 버튼으로 작동.
2. 수식 처리
   - 괄호가 포함된 수식은 괄호의 우선 순위에 따라 정확하게 계산.
3. 연산 순서
   - 연산자 우선순위(예: 곱셈과 나눗셈이 덧셈과 뺄셈보다 먼저 수행됨)를 고려하여 계산.
4. 지우기/ 리셋 기능
   - 입력된 수식이나 현재의 계산 결과를 지우기/리셋할 수 있는 버튼 제공
5. 소수점 및 음수 처리
   - 소수점 중복 사용 여부 체크. (연산자가 있을 때와 없을 때도 포함)
   - 음수는 빈 문자열일 때도 입력될 수 있도록 처리.
   - 소수점 및 음수 계산 가능.
6. 연산 기록 기능
   - 사용자가 수행한 최근 연산 기록을 화면에 표시.
   - 연산 기록 클릭 시 해당 기록을 계산에 활용하는 기능.
7. 키보드 입력 지원
   - 키보드를 사용한 입력이 가능.


#### 수정하지 못한 버그
1. RichTextBox의 길이보다 입력 값의 길이가 길 경우, 입력 값이 갱신되지 않는 버그
2. 입력한 값을 계산하고 오류가 발생할 시, 계산 기록이 초기화되는 버그
3. 표기할 수 있는 숫자의 범위를 초과할 경우 오류가 발생하는 버그


💻**프로그래밍 언어**
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

🪛**개발 환경 및 도구**
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)

🕸️**웹 개발 및 프레임워크**
<img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.NET&logoColor=white"> (8.0 버전 사용)

✍️ **버전 관리**
 <img src="https://img.shields.io/badge/github-181717?style=for-the-badge&logo=github&logoColor=white">
