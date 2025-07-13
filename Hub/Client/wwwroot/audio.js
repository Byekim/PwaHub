let audioElement;

function playAudio(base64Audio) {
    if (!audioElement) {
        audioElement = new Audio();
    }

    audioElement.src = `data:audio/mpeg;base64,${base64Audio}`;
    audioElement.play();
}
