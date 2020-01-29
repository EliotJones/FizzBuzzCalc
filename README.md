# FizzBuzz Using Windows 10 Calculator #

This is a joke project to test the feasibility of solving FizzBuzz using the Windows 10 Calculator.

This uses the [FlaUI](https://github.com/FlaUI/FlaUI) project to automate interaction with the Calculator App. Since the Windows 10 calculator app is now a Metro-style Store App rather than a native Windows app it is harder to get an interactive instance of the application and relies on a hard-coded application name. An improvement may be to locate the application name in the registry but it's unclear how to do this for the Calculator.

This solution is running .NET Core 2.1 and has been tested only with:
+ Microsoft Windows 10 Version 1903 (x64).
+ Visual Studio 2017 Community.
+ en-US locale.

The application runs for values 1 to 52. The rules of FizzBuzz are:
+ If a number is divisible by 3 print "Fizz".
+ If a number is divisible by 5 print "Buzz".
+ If a number is divisible by 3 and 5 print "FizzBuzz".
+ If a number isn't divisible by either 3 or 5 print the number.

The code launches a new Windows 10 Calculator Process then for each number in the range, divides it first by 3 and then by 5. If the result contains decimal places then it is not divisible by the divisor.

This is a joke application and aims to be among the worst ways to solve the common FizzBuzz interview question.

An example of the output is shown below:

![Application run through to 15](https://raw.githubusercontent.com/EliotJones/FizzBuzzCalc/master/docs/example-output.png)

You can also watch in action here: https://www.youtube.com/watch?v=L-vJ6ov4Cac&feature=youtu.be

## Disclaimers ##
+ **Running The Application Will Capture The Mouse For The Duration Of The Run.**.
+ **Running The Application Will Close Any Open Calculator Processes.**
+ **Minimising The Calculator Will Cause Wrong Results.**