<Query Kind="Program" />

void Main()
{
	try
	{
		//  passing in an empty first and last name
		AggregateExceptionTest("", "");
	}

	#region catch all exception
	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	#endregion
}

public Exception GetInnerException(System.Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}

public void AggregateExceptionTest(string firstName, string lastName)
{
	#region Business Logic and Parameter Exceptions
	//	create a List<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();

	//	Business Rules
	//	There are processing rules that need to be satisfied
	//		for valid data
	//		rule: first name cannot be empty or null
	//		rule: last name cannot be empty or null

	//	parameter validation
	if (string.IsNullOrWhiteSpace(firstName))
	{
		errorList.Add(new Exception("First name is required and cannot be empty"));
	}

	if (string.IsNullOrWhiteSpace(lastName))
	{
		errorList.Add(new Exception("Last name is required and cannot be empty"));
	}
	#endregion

	/*
		actual code for method
	*/

	#region Saving
	if (errorList.Count() > 0)
	{
		//	throw the list of business processing error(s)
		throw new AggregateException("Unable to proceed! Check concerns", errorList);
	}
	#endregion
}
