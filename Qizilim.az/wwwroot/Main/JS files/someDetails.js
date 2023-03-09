const typeDetails = document.querySelector(".type");
const diamondDetails = document.querySelector('.diamond');
const locDetails = document.querySelector(".loc");
const deliveryDetails = document.querySelector(".delivery");



typeDetails.querySelectorAll("ul li").forEach(li => {
    li.addEventListener('click', () => {
        typeDetails.removeAttribute("open")
    })
})

diamondDetails.querySelectorAll("ul li").forEach(li => {
    li.addEventListener('click', () => {
        diamondDetails.removeAttribute("open")
    })
})
locDetails.querySelectorAll("ul li").forEach(li => {
    li.addEventListener('click', () => {
        locDetails.removeAttribute("open")
    })
})
deliveryDetails.querySelectorAll("ul li").forEach(li => {
    li.addEventListener('click', () => {
        deliveryDetails.removeAttribute("open")
    })
})
