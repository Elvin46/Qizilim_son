function importDataUser() {
  let input = document.createElement('input');
  input.type = 'file';
  input.accept="image/*";
//   <input type="file" class="file-field" id="images" accept="image/*" multiple="multiple" required>

  input.onchange = _ => {
    // you can use this method to get file and perform respective operations
            let files =   Array.from(input.files);
            console.log(files);
        };
  input.click();
  
}
