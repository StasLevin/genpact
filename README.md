# Genpact Automation Infrastructure Assignment

## Objective
A lightweight automation framework with one test that compares **UI** and **API** behavior for the [Playwright Wikipedia page](https://en.wikipedia.org/wiki/Playwright_(software)) — specifically, the **"Debugging features"** section.

## Task
1. Extract the **"Debugging features"** section:
   - Via **UI** using the Page Object Model (POM) approach.
   - Via **API** using the MediaWiki Parse API.
2. Normalize both texts.
3. Count **unique words** in each.
4. **Assert** that both counts are equal.

## Test Result
1. HTML report generated.

## Tech Stack
- **Programming Languages:** C#, JS
- **Test Automation:** Playwright

## How to Run
1. Clone the repository:
   ```bash
   git clone git@github.com:StasLevin/genpact.git
   cd genpact
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. How to run
   1. Run the tests:
   ```bash
   dotnet test
   ```
   2. Run the tests with console output:
   ```bash
   dotnet test --logger "console;verbosity=detailed"
   ```

4. Open HTML report
   - HTML Report generated under the *TestReports* folder under Project Root
   - There is no option to see the run history. Just the last run available

5. Misc
   Developed and tested on macOS only
