
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
@TypeOfUser INT,
@IsActive BIT=1
AS
BEGIN
	SET NOCOUNT  ON;

	IF @Email IS NULL OR @Password IS NULL OR @TypeOfUser IS NULL 
	BEGIN 
		RAISERROR('Missing required parameters',16,1);
		RETURN;
	END
	
	IF @IsActive IS NULL
	BEGIN
		SET @IsActive = 1;
	END

	IF EXISTS(SELECT 1 FROM Users WHERE EMAIL=@Email)
	BEGIN
		RAISERROR('The user already exists',16,1);
		RETURN;
	END

	BEGIN TRY
		INSERT INTO Users([Email],[PasswordHash],[UserType],[IsActive])
		VALUES (@Email,@Password,@TypeOfUser,@IsActive);

		DECLARE @NewId INT=SCOPE_IDENTITY();

		SELECT 
			@NewId as Id,
			@Email as Email,
			@TypeOfUser as TypeOfUser,
			NULL AS FirstName,
			NULL AS LastName,
			Null AS Birthday,
			NULL AS ProfilePicture,
			@IsActive as IsActive;
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
		SELECT U.Id, U.Email, U.UserType,U.FirstName,U.LastName,U.Birthday,U.ProfilePicture,u.ISActive FROM Users U WHERE @Email=U.Email AND @Password=U.PasswordHash
	END TRY

	BEGIN CATCH
		THROW;
	END CATCH
END 




CREATE PROCEDURE StoredPatchUser
@Id INT,
@FirstName NVARCHAR(100)=NULL,
@LastName NVARCHAR(100)=NULL,
@Birthday DATE=NULL,
@ProfilePicture NVARCHAR(500)=NULL
AS
BEGIN
	IF @Id IS NULL
	BEGIN
		RAISERROR ('The Id of the user is missing',16,1);
		RETURN;
	END

	BEGIN TRY
	UPDATE Users
	SET
		FirstName=ISNULL(@firstName,FirstName),
		LastName=ISNULL(@LastName,LastName),
		Birthday=ISNULL(@Birthday,Birthday),
		PROFILEPICTURE=ISNULL(@ProfilePicture,ProfilePicture)
		WHERE Id=@Id;

	SELECT U.Id,U.Email,U.UserType,U.FirstName,U.LastName,U.Birthday,U.ProfilePicture,U.ISActive FROM Users U WHERE U.Id=@Id;

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;
END


CREATE PROCEDURE GetAll
@Skip INT=0,
@Take INT=10 
as
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		SELECT COUNT(*) AS TotalCount FROM USERS;
		
		SELECT U.Id,U.Email,U.UserType,U.FirstName,U.LastName,U.Birthday,U.ProfilePicture,U.ISActive FROM Users U
		ORDER BY U.Id
		OFFSET @Skip ROWS
		FETCH NEXT @Take ROWS ONLY;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


CREATE PROCEDURE GetById
@Id int
as
BEGIN
	BEGIN TRY
		SELECT U.Id,U.Email,U.UserType,U.FirstName,U.LastName,U.Birthday,U.ProfilePicture,U.ISActive FROM Users U WHERE Id=@Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


CREATE PROCEDURE EmailExists
@Email NVARCHAR(255)
AS
BEGIN
	IF (@Email IS NULL OR LTRIM(RTRIM(@Email))='')
	BEGIN
		RAISERROR('The email is null or empty',16,1);
		return;
	END

	BEGIN TRY
		IF EXISTS(SELECT 1 FROM Users U WHERE Email=@Email)
			SELECT CAST(1 AS BIT) AS ItExists ;
		ELSE
			SELECT CAST(0 AS BIT) AS ItExists ;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END






