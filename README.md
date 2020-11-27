![Build](https://github.com/Abc-Arbitrage/Abc.MoqComplete/workflows/Build/badge.svg)

# Abc.MoqComplete
MoqComplete is a Resharper plugin which provide auto-completion for the Moq framework (and Moq.AutoMock)<br/>
Works with latest Resharper 2020 and Rider (**non EAP**)

[<p align="center"><img src="Media/jetbrains.svg"></p>](https://www.jetbrains.com/?from=MoqComplete)

## Features
### It.IsAny completion
Suggest `It.IsAny()` when setting up mocked method

![](Media/ItIsAny_SetupCompletion.gif)

Suggest `It.IsAny()` when using verify on mocked method

![](Media/ItIsAny_VerifyCompletion.gif)

### Callback Completion
Suggest full `Callback<...>` method

![](Media/CallbackCompletion.gif)

### Returns Completion
Suggest full `Returns<...>` method

![](Media/ReturnsCompletion.gif)

### Suspicious Callback Detection
Detect suspicious `Callback`

![](Media/SuspiciousCallback.gif)

### Mock suggestion
Suggest existing `mock.Object`

![](Media/MockCompletion.gif)

Or new `Mock` in constructor

![](Media/MockProposalCompletion.gif)

### Fill with Mock
Fill an object with `Mock` objects (using fields or local variables)

![](Media/fillWithMock.gif)

### Mock variable name completion
Suggest the mock variable name

![](Media/proposeMockVarName.gif)

## Installation
The plugin can be installed using the extension manager from Resharper menu in Visual Studio, or using the plugin menu from Rider settings
