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

window.isDemoNoticeDismissed = () => localStorage.getItem("demoNoticeDismissed") === "true";
window.dismissDemoNotice = () => localStorage.setItem("demoNoticeDismissed", "true");

window.isCookieNoticeAccepted = () => localStorage.getItem("cookieNoticeAccepted") === "true";
window.acceptCookieNotice = () => localStorage.setItem("cookieNoticeAccepted", "true");

window.authPost = async (url, body) => {
  const response = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json", "Accept": "application/json" },
    credentials: "same-origin",
    body: JSON.stringify(body),
  });

  let payload = null;
  const text = await response.text();
  if (text) {
    try {
      payload = JSON.parse(text);
    } catch {
      payload = { errors: [text] };
    }
  }

  return {
    ok: response.ok,
    status: response.status,
    payload,
  };
};
