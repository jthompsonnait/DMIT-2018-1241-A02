<Query Kind="Program">
  <Connection>
    <ID>b375a2b5-7c71-41ae-b70e-7f621b6a5f46</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	try
	{
		//  passing in an empty first and last name
		// AggregateExceptionTest("", "");

		//	passing an track id that is larger than the max TrackID
		// ArgumentNullExceptionTest(10000);
		
		//	passing an invalid track ID (less than 1)
		ExceptionTest(0);
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


public void ArgumentNullExceptionTest(int trackID)
{
	#region Business Logic and Parameter Exceptions
	//	create a List<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();

	//	Business Rules
	//	There are processing rules that need to be satisfied
	//		for valid data
	//		rule: Track must exist in the database

	//	parameter validation

	var track = Tracks
				.Where(x => x.TrackId == trackID)
				.Select(x => x).FirstOrDefault();
	if (track == null)
	{
		throw new ArgumentNullException($"No track was found for Track ID: {trackID}");
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

public void ExceptionTest(int trackID)
{
	#region Business Logic and Parameter Exceptions
	//	create a List<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();

	//	Business Rules
	//	There are processing rules that need to be satisfied
	//		for valid data
	//		rule: Track must be valid
	//	parameter validation
	if (trackID < 1)
	{
		throw new Exception($"TrackID is invalid: {trackID}");
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
