const forgotPasswordBtn = document.getElementById("forgot_password");
const registersBtn = document.querySelectorAll(".register-btn");
const signinBtn = document.querySelector(".signin-btn");
const email = document.getElementById("email");
const emailText = document.getElementById("emailText");
const submitBtn = document.getElementById("submit");
submitBtn.addEventListener("click", (e) => {
  e.preventDefault();
  const email = localStorage.setItem("Email", emailText.value);
});
email.addEventListener("keyup", (e) => console.log(e.target.value.length));
email.addEventListener("focus", () => {
  document.getElementById("email_txt").style.top = "-5px";
});
email.addEventListener("focusout", () => {
  console.log(email.value);
  if (email.value.length > 0) {
    document.getElementById("email_txt").style.top = "-5px";
  } else if (email.value.length == 0) {
    document.getElementById("email_txt").style.top = "50%";
  }
});
//signinBtn.addEventListener("click", (e) => {
//  e.preventDefault();
//  document
//    .querySelector(".login_register_forgot")
//    .querySelector(".center").style.display = "block";
//  document
//    .querySelector(".login_register_forgot")
//    .querySelector(".center").nextElementSibling.style.display = "none";
//  document
//    .querySelector(".login_register_forgot")
//    .querySelector(
//      ".center"
//    ).nextElementSibling.nextElementSibling.style.display = "none";
//});
//registersBtn.forEach((registerBtn) => {
//  registerBtn.addEventListener("click", (e) => {
//    e.preventDefault();
//    document
//      .querySelector(".login_register_forgot")
//      .querySelector(".center").style.display = "none";
//    document
//      .querySelector(".login_register_forgot")
//      .querySelector(".center").nextElementSibling.style.display = "none";
//    document
//      .querySelector(".login_register_forgot")
//      .querySelector(
//        ".center"
//      ).nextElementSibling.nextElementSibling.style.display = "block";
//  });
//});
forgotPasswordBtn.addEventListener("click", (e) => {
  e.preventDefault();
  document
    .querySelector(".login_register_forgot")
    .querySelector(".center").style.display = "none";
  document
    .querySelector(".login_register_forgot")
    .querySelector(".center").nextElementSibling.style.display = "block";
});
