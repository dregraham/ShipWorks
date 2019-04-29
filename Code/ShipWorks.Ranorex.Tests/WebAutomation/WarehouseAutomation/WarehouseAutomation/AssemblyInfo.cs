using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]

//this code is required by the Xunit Ordering extension
//reference on how to use the extension: https://github.com/tomaszeman/Xunit.Extensions.Ordering#test-cases-ordering