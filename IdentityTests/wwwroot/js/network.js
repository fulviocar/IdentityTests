const baseUrl = "https://localhost:7035/api/data";
const authUrl = "https://localhost:7035/api/auth";

const baseRequestConfig = {
    credentials: "include"
}

export const signIn = async function (email, password, callback,
    errorHandler) {
    const response = await fetch(`${authUrl}/signin`, {
        ...baseRequestConfig,
        method: "POST",
        body: JSON.stringify({ email, password }),
        headers: { "Content-Type": "application/json" }
    });
    processResponse(response, async () =>
        callback(await response.json()), errorHandler);
}

export const signOut = async function (callback) {
    const response = await fetch(`${authUrl}/signout`, {
        ...baseRequestConfig,
        method: "POST"
    });
    processResponse(response, callback, callback);
}

export const loadData = async function (callback, errorHandler) {
    const response = await fetch(baseUrl, {
        ...baseRequestConfig,
        redirect: "manual"
    });
    processResponse(response, async () =>
        callback(await response.json()), errorHandler);
}

function processResponse(response, callback, errorHandler) {
    if (response.ok) {
        callback();
    } else {
        errorHandler(response.status);
    }
}