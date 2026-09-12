// Photino (the desktop shell) injects window.external.sendMessage on every page.
// When it is present the host process handles saving via a native dialog, because
// WebKit-based webviews (Linux/macOS) cannot download blob: URLs.
const isDesktopHost = () =>
    typeof window.external === "object" &&
    window.external !== null &&
    typeof window.external.sendMessage === "function";

const toBase64 = (bytes) => {
    // Chunked to avoid "Maximum call stack size exceeded" on large arrays
    let binary = "";
    const chunk = 0x8000;
    for (let i = 0; i < bytes.length; i += chunk) {
        binary += String.fromCharCode.apply(null, bytes.subarray(i, i + chunk));
    }
    return btoa(binary);
};

window.promptFilename = (defaultName) => {
    // The desktop host asks for the filename in its native Save dialog instead
    if (isDesktopHost())
        return defaultName;
    return prompt("Enter a filename:", defaultName) || null;
};

window.downloadPdf = (fileName, byteData) => {
    const bytes = new Uint8Array(byteData);

    if (isDesktopHost()) {
        window.external.sendMessage(JSON.stringify({
            type: "savePdf",
            fileName: fileName,
            data: toBase64(bytes)
        }));
        return;
    }

    let blob = new Blob([bytes], { type: "application/pdf" });
    let url = URL.createObjectURL(blob);

    let a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

    URL.revokeObjectURL(url);
};
