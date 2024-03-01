/* dark mode setting */
window.enableDarkClass = function () {
    document.documentElement.classList.add("dark");
    document.body.classList.add("fadeindarkclass");
    document.body.classList.remove("fadeinlightclass");
}
window.disableDarkClass = function () {
    document.documentElement.classList.remove("dark");
    document.body.classList.add("fadeinlightclass");
    document.body.classList.remove("fadeindarkclass");
}

/* textarea setting */
window.autoResizeTextarea = function (className) {
    var textareas = document.querySelectorAll(`.${className}`);
    textareas.forEach((textarea) => {
        textarea.style.height = 'auto';
        textarea.style.height = `${textarea.scrollHeight}px`;
    });
};

/* file drop zone setting */
window.changeFileDropZoneText = function (filenames) {
    clearFileDropZoneText();

    if ((filenames !== null || filenames !== "") && Array.isArray(filenames)) {
        // remove guide text
        var guideElement = document.getElementById("guide");
        if (guideElement) {
            guideElement.parentNode.removeChild(guideElement);
        }

        // create element for file names
        var filesNamesElement = document.createElement("div"); document.getElementById("selected-files");
        filesNamesElement.id = "selected-files";

        // on a per-file-name  basis
        filenames.forEach((filename) => {
            // create new element with file name
            var fileNameParagraph = document.createElement("p");
            fileNameParagraph.className = "mb-2 text-sm font-bold text-gray-500 dark:text-gray-400";
            fileNameParagraph.textContent = filename;

            // add element with file name to element for file names
            filesNamesElement.appendChild(fileNameParagraph);
        });

        var dropzoneElement = document.getElementById("dropzone");
        if (dropzoneElement) {
            // add element for file names to drop zone
            dropzoneElement.appendChild(filesNamesElement);
        }
    }
}

/* file drop zone clear button setting */
window.clearFileDropZoneText = function () {
    // remove element for file names
    var filesNamesElement = document.getElementById("selected-files");
    if (filesNamesElement) {
        var parentElement = filesNamesElement.parentElement;
        if (parentElement) {
            parentElement.removeChild(filesNamesElement);

            // create new element for guide
            var guideElement = document.createElement("div");
            guideElement.id = "guide";
            var guide = document.createElement("p");
            guide.className = "mb-2 text-sm text-gray-500 dark:text-gray-400";
            guide.textContent += "click here  to attach files";

            // add element for guide to drop zone
            guideElement.appendChild(guide);
            var dropzoneElement = document.getElementById("dropzone");
            dropzoneElement.appendChild(guideElement);
        }
    }
}

/* modal setting */
window.showModal = function () {
    var modal = document.getElementById('popup-modal');
    modal.classList.remove('hidden');
};
window.hideModal = function () {
    var modal = document.getElementById('popup-modal');
    modal.classList.add('hidden');
};
