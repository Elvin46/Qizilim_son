const walletBTN = document.querySelector(".wallet-button");
const ClosePopup = document.querySelector(".close-popup");

walletBTN.addEventListener("click", () => {
    document.querySelector(".premium-popup").style.display = "block"
})

ClosePopup.addEventListener("click", () => {
    document.querySelector(".premium-popup").style.display = "none"
})