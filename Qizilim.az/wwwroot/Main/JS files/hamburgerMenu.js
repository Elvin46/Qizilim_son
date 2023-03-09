
const hamburgerMenu = document.getElementById("hamburger-menu");

hamburgerMenu.addEventListener("click", (e) => {
    console.log(e.target);
    sidebarMenu.style.visibility = "visible";
    sidebarMenu.style.left = "0";
});
document.getElementById("close-menu-btn").addEventListener("click", () => {
    sidebarMenu.style.visibility = "hidden";
    sidebarMenu.style.left = "-250px";
});