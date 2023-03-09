function EnableDisableTextBox(diamonCheck) {
  var addDiamondCount = document.getElementById("addDiamondCount");
  addDiamondCount.disabled = diamonCheck.checked ? false : true;
  if (!addDiamondCount.disabled) {
    addDiamondCount.focus();
  }
}
