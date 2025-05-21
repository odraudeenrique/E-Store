export class User {

    constructor(email, password, userType = -1) {
        //-1 is the invalid user. There an Enum on the api that define that
        this.email = email;
        this.password = password;
        this.TypeOfUser = userType;
    }

}