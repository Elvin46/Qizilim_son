function importDataShop() {
  let input = document.createElement('input');
  input.type = 'file';
  input.accept = "image/*";
  //   <input type="file" class="file-field" id="images" accept="image/*" multiple="multiple" required>

  input.onchange = _ => {
    // you can use this method to get file and perform respective operations
    let files = Array.from(input.files);
    console.log(files[0]);
    const reader = new FileReader();
    reader.addEventListener("load", () => {
      document.querySelector(".brand-image-edit").src = reader.result;
    });
    reader.readAsDataURL(files[0]);
  };
  input.click();

}
