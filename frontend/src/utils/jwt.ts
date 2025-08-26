export function decodeJWT(token: string) {
  try {
    const payload = token.split(".")[1];
    const decoded = atob(payload);
    return JSON.parse(decoded);
  } catch (error) {
    console.error("JWT decode hatası:", error);
    return null;
  }
}
