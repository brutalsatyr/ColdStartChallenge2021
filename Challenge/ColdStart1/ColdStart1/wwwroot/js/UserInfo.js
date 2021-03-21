async function getUserInfo() {
    try {
        const response = await fetch('/.auth/me');
        const payload = await response.json();
        const { clientPrincipal } = payload;
        console.log(clientPrincipal);
        return clientPrincipal;
    } catch (error) {
        console.error('No profile could be found');
        return undefined;
    }
}