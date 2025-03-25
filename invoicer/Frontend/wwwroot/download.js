window.promptFilename = (defaultName) => {
    return prompt("Enter a filename:", defaultName) || null;
};

window.downloadPdf = (fileName, byteData) => {
    let blob = new Blob([new Uint8Array(byteData)], { type: "application/pdf" });
    let url = URL.createObjectURL(blob);

    let a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

    URL.revokeObjectURL(url);
};