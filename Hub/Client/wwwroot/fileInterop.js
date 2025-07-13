window.fileInterop = {
    openFile: async function () {
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = '*/*';  // 모든 파일 형식을 허용

        return new Promise((resolve, reject) => {
            input.onchange = async (event) => {
                const file = input.files[0];  // 첫 번째 파일을 선택
                if (file) {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        // 파일을 byte 배열로 변환
                        const byteArray = new Uint8Array(e.target.result);
                        // 파일 내용을 반환 (base64 형식 또는 다른 방식)
                        resolve(file.name);  // 파일 이름을 반환
                    };
                    reader.onerror = reject; // 읽기 오류 발생 시 reject
                    reader.readAsArrayBuffer(file);  // 파일 내용을 ArrayBuffer로 읽음
                } else {
                    reject("No file selected");
                }
            };
            input.click();  // 파일 선택창을 엶
        });
    },

    saveFile: function (content, fileName) {
        const blob = new Blob([content], { type: 'text/plain' }); // content를 Blob으로 변환
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = fileName;  // 다운로드할 파일 이름 설정
        link.click();  // 다운로드 실행
    },

    downloadFile: function (fileName, content) {
        const blob = new Blob([content], { type: "audio/mpeg" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

};
