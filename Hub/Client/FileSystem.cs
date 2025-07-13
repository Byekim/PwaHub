using Microsoft.JSInterop;

namespace Hub.Client
{
    public class FileSystemService
    {
        private readonly IJSRuntime _jsRuntime;
        
        public FileSystemService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> OpenFileAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("openFile");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open file.", ex);
            }
        }

        public async Task SaveFileAsync(string content)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("saveFile", content);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save file.", ex);
            }
        }
    }

}
