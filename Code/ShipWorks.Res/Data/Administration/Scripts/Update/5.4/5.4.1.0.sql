
ALTER TABLE [dbo].[ActionQueue] ADD [ExtraData] [xml] NULL
GO

-- Create finish shipment processing batch action

DECLARE @ActionID BIGINT

INSERT INTO [dbo].[Action] ([Name], [Enabled], [ComputerLimitedType], [ComputerLimitedList], [StoreLimited], [StoreLimitedList], [TriggerType], [TriggerSettings], [TaskSummary], [InternalOwner])
VALUES (N'Finish Processing Batch', 1, 1, '', 0, N'', 8, CONVERT(xml,N'<Settings>
  <RestrictStandardReturn value="True" />
  <ReturnShipmentsOnly value="False" />
  <RestrictType value="False" />
</Settings>',1), N'FinishProcessingBatch', N'FinishProcessingBatch')
SELECT @ActionID = SCOPE_IDENTITY()

PRINT (N'Add 1 row to [dbo].[ActionTask]')
INSERT INTO [dbo].[ActionTask] ([ActionID], [TaskIdentifier], [TaskSettings], [StepIndex], [InputSource], [InputFilterNodeID], [FilterCondition], [FilterConditionNodeID], [FlowSuccess], [FlowSkipped], [FlowError])
VALUES (@ActionID, N'FinishProcessingBatch', CONVERT(xml, N'<Settings />', 1), 0, -1, -1, 0, -1, 0, 0, 0)
GO
