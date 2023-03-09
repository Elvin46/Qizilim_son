
const bePremimumBtn = document.getElementById("be-premium");
const popupBtns = document.querySelectorAll(".premium-popup");
const closePopUps = document.querySelectorAll(".close-popup");
const beFirst = document.getElementById("be-first");

beFirst.addEventListener("click", () => {
    popupBtns[1].style.display = "block"
})
bePremimumBtn.addEventListener("click", (e) => {

    popupBtns[0].style.display = "block"

})

closePopUps.forEach((closepopup, index) => {
    closepopup.addEventListener("click", () => {
        popupBtns[index].style.display = "none"
    })
})