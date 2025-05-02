CREATE PROCEDURE StoredToInsertError
@Title NVARCHAR (255),
@Message NVARCHAR(MAX),
@StackTrace NVARCHAR(MAX),
@Source NVARCHAR (255),
@DateCreated Datetime
AS
BEGIN
INSERT INTO ERRORLOGS([Title],[Message],[StackTrace],[Source],[DateCreated])
VALUES(@Title,@Message,@StackTrace,@Source,GETDATE())
END



CREATE PROCEDURE StoredToCreateUse
@Email NVARCHAR(255),
@Password NVarchar(44),
@UserType INT 
AS
Begin 
	SET NOCOUNT ON;
	IF @Email IS NULL OR @Password IS NULL OR @UserType IS NULL
	BEGIN
		SELECT CAST('Missing required parameters' AS NVARCHAR(100));
		RETURN;
	END

	IF EXISTS(SELECT 1 FROM Users WHERE Email=@Email)
	BEGIN 
		SELECT CAST('The email already exists' AS NVARCHAR(100) );
		return;
	END

	BEGIN TRY
		INSERT INTO Users ([Email],[PasswordHash],[UserType])
		VALUES(@Email,@Password,@UserType)

		SELECT CAST('User created successfully' AS NVARCHAR(100) );
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


