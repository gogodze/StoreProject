// toggle from dark to light
window.toggleTheme = () => {
  const theme = localStorage.theme || "light";
  localStorage.theme = theme === "light" ? "dark" : "light";

  document.documentElement.classList.toggle("dark", localStorage.theme === "dark");
  document.documentElement.classList.toggle("light", localStorage.theme === "light");
}

// on load, set theme to whatever the user set before, or what the browser prefers.
window.addEventListener("load", () => {
  document.documentElement.classList.toggle(
    "dark",
    localStorage.theme === "dark" ||
    (!("theme" in localStorage) && window.matchMedia("(prefers-color-scheme: dark)").matches),
  );
});

window.toggleNav = () => {
  document.getElementById('nav-left').classList.toggle('hidden')
}

window.demoNotice = {
  key: "demoNoticeDismissed",
  isDismissed: () => localStorage.getItem(window.demoNotice.key) === "true",
  dismiss: () => localStorage.setItem(window.demoNotice.key, "true"),
};

window.cookieNotice = {
  key: "cookieNoticeAccepted",
  isAccepted: () => localStorage.getItem(window.cookieNotice.key) === "true",
  accept: () => localStorage.setItem(window.cookieNotice.key, "true"),
};
