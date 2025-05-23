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



CREATE PROCEDURE StoredToCreateUser
@Email NVARCHAR(255),
@Password NVARCHAR(44),
@TypeOfUser INT
AS
BEGIN
	SET NOCOUNT  ON;
	IF @Email IS NULL OR @Password IS NULL OR @TypeOfUser IS NULL
	BEGIN 
		RAISERROR('Missing required parameters',16,1);
		RETURN;
	END
	
	IF EXISTS(SELECT 1 FROM Users WHERE EMAIL=@Email)
	BEGIN
		RAISERROR('The user already exists',16,1);
		RETURN;
	END

	BEGIN TRY
		INSERT INTO Users([Email],[PasswordHash],[UserType])
		VALUES (@Email,@Password,@TypeOfUser);

		DECLARE @NewId INT=SCOPE_IDENTITY();

		SELECT 
			@NewId as Id,
			@Email as Email,
			@TypeOfUser as TypeOfUser;
	END TRY

	BEGIN CATCH
		THROW;
	END CATCH
END


CREATE PROCEDURE StoredToLogin 
@Email NVARCHAR(255),
@Password NVARCHAR(44)
AS
BEGIN 
	IF @Email IS NULL OR @Password IS NULL
	BEGIN 
		RAISERROR('Missing required parameters',16,1);
		return;
	end
	
	BEGIN TRY
		SELECT U.Id, U.Email, U.UserType FROM Users U WHERE @Email=U.Email AND @Password=U.PasswordHash
	END TRY

	BEGIN CATCH
		THROW;
	END CATCH
END 



