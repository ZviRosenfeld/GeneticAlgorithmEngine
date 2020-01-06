Want to fix a bug or add a new feature? That's great.

Before stating, you should open an issue so that everyone's on the same page. Mention in the issue that you'd like to solve it, and outline your suggested solution.

Please create a new branch to work on. It's not critical that you give the branch a meaningful name, but it's better if you do.

When you've finished with the development, push it back to github and create a new pull-request from your branch to master. Then, someone will either approve your PR and merge it with master, or explain what you should fix.

Coding Conventions
Names should be meaningful (e.g. use time, and not t).
Don't use cruelly brakes for one line conditions or loops.
Public property names should start with an uppercase letter (e.g. Property).
Private fields should start with a lowercase letter (e.g. field).
Methods should always start with an uppercase letter (e.g. Method).
When a method or variable name contains more then one word, use uppercase letter to split the words (twoWords).
Unit Tests
If subtitle, you should add unit tests that cover your new code. In this project MsTest is used as a unit test framework, and FakeItEasy as a mocking framework. Please don't use other frameworks for these purpose.
