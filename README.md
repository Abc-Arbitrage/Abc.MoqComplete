# Abc.MoqComplete
MoqComplete is a Resharper plugin which provide auto-completion for the Moq framework<br/>
Works with Resharper 2018.3.1

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

### Mock suggestion
Suggest existing `mock.Object` (Works only with Nunit and MSTests)

![](Media/MockCompletion.gif)

Or new `Mock` in constructor

![](Media/MockProposalCompletion.gif)


## About
Inspired by AgentZorge, which is deprecated
