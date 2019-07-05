[![Build Status](https://dev.azure.com/lconstantin0753/Abc.MoqComplete/_apis/build/status/Abc-Arbitrage.Abc.MoqComplete?branchName=master)](https://dev.azure.com/lconstantin0753/Abc.MoqComplete/_build/latest?definitionId=2&branchName=master)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/lconstantin0753/Abc.MoqComplete/2.svg)

# Abc.MoqComplete
MoqComplete is a Resharper plugin which provide auto-completion for the Moq framework<br/>
Works with Resharper 2019.1 and Rider

## Features
### It.IsAny completion
Suggest `It.IsAny()` when setting up mocked method

![](Media/ItIsAny_SetupCompletion.gif)

Suggest `It.IsAny()` when using verify on mocked method

![](Media/ItIsAny_VerifyCompletion.gif)

### Callback Completion
Suggest full `Callback<...>` method

![](Media/CallbackCompletion.gif)

### Suspicious Callback Detection
Detect suspicious `Callback`

![](Media/SuspiciousCallback.gif)

### Returns Completion
Suggest full `Returns` method

![](Media/ReturnsCompletion.gif)

### Mock suggestion
Suggest existing `mock.Object`

![](Media/MockCompletion.gif)

Or new `Mock` in constructor

![](Media/MockProposalCompletion.gif)

### Fill with Mock
Fill an object with `Mock` objects
![](Media/fillWithMock.gif)

## About
Inspired by AgentZorge, which is deprecated
