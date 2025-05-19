import { User } from "../Models/user.model";

export class UserService {

    static async  createUser(email, password) {
        const url = "https://localhost:5001/api/user";

        if (email === null || email === undefined || email === "" || password === null || password === undefined || password==="") {
            return; 
        }

        const regularUser = 1;
        const user = { email, password, regularUser };

        const request = {
            method: "POST",
            headers: { "content-type": "application/json" },
            body: JSON.stringify(user)
        };

        const response = await fetch(url, request);

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || "Unknown server error");
        }
        const contentLength = response.headers.get("content-length");

        if (!contentLength || parseInt(contentLength) === 0) {
            throw new Error("Empty response from the server");
        }

        const aux = await response.json();

        if (!aux) {
            return ("Invalid user");
        }

        return aux;
        
    }



}