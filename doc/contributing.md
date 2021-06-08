# Contribution Guidelines

The project follows the Git flow to ensure traceability, reduce conflicts, and improve the project management. Traceability is introduced by being pedantic on filing issues, such that any feature branch has a corresponding issue.

The following guide should help you getting on track:

1. If no issue already exists for the work you'll be doing, create one to document the problem(s) being solved and self-assign the issue.
2. Otherwise, please let us know that you are working on the problem. Regular status updates (e.g., "still in progress", "no time anymore", "practically done", "pull request issued") are highly welcome. A possible code-complete date helps us placing the issue in the overall roadmap.
2. Create a new branch! Please don't work in the `main` branch directly. It is reserved for stable versions, i.e., releases. We recommend naming the branch to match the issue being addressed (`feature-#777` or `issue-777`), but we accept all names except `main` and `devel`.
3. Add failing tests for the change you want to make. Tests are crucial and should cover the code involved in the issue.
4. Fix stuff. Always go from edge case to edge case.
5. All tests should pass now. Also your new implementation should not break existing tests.
6. Update the documentation to reflect any potential changes.
7. Push to your fork or push your issue-specific branch to the main repository, then submit a pull request against `devel`. Never create a PR against the `main` branch!

TL;DR: There are two fixed branches, `main` and `devel`, which should not be touched directly. `main` contains the current stable version, i.e., the last released version, `devel` contains the working version, which is the branch aggregating all feature branches.
