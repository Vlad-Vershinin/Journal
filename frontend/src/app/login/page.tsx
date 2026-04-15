"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { User, Lock, Eye, EyeOff } from "lucide-react";
import ThemeToggle from "../components/ThemeToggle";
import * as jose from "jose";

export default function LoginPage() {
  const router = useRouter();

  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      const res = await fetch("http://localhost:5280/api/user/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ login, password }),
      });

      if (!res.ok) {
        throw new Error("Неверный логин или пароль");
      }

      const token = await res.text().then(t => t.replace(/^"|"$/g, ''));

      localStorage.setItem("token", token);

      const payload = jose.decodeJwt(token);

      const role = payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

      if (role === "Admin") {
        router.push("/admin");
      } else {
        router.push("/");
      }
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-primary-50 dark:bg-background-dark px-4 relative">
      <div className="absolute top-4 right-4">
        <ThemeToggle />
      </div>

      <div className="w-full max-w-md">
        <div className="bg-white dark:bg-surface-dark rounded-2xl shadow-card dark:shadow-none dark:border dark:border-border-dark p-10">
          <div className="text-center mb-8">
            <div className="inline-flex items-center justify-center w-14 h-14 mb-4 rounded-full bg-primary-600 dark:bg-primary-500">
              <span className="text-2xl font-bold text-white">J</span>
            </div>
            <h1 className="text-2xl font-semibold text-text-primary dark:text-text-dark-primary">
              Journal
            </h1>
          </div>

          {error && (
            <div className="mb-5 text-sm text-red-700 dark:text-red-300 bg-red-50 dark:bg-red-950/30 border border-red-200 dark:border-red-900 p-3 rounded-lg">
              {error}
            </div>
          )}

          <form onSubmit={handleLogin} className="space-y-4">
            <div>
              <div className="relative">
                <div className="absolute left-3.5 top-1/2 -translate-y-1/2 text-text-muted dark:text-text-dark-muted">
                  <User size={18} />
                </div>
                <input
                  type="text"
                  value={login}
                  onChange={(e) => setLogin(e.target.value)}
                  placeholder="Логин"
                  className="w-full pl-11 pr-4 py-3 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary placeholder:text-text-muted dark:placeholder:text-text-dark-muted text-base outline-none focus:bg-white dark:focus:bg-surface-dark focus:border-primary-500 dark:focus:border-primary-400"
                />
              </div>
            </div>

            <div>
              <div className="relative">
                <div className="absolute left-3.5 top-1/2 -translate-y-1/2 text-text-muted dark:text-text-dark-muted">
                  <Lock size={18} />
                </div>
                <input
                  type={showPassword ? "text" : "password"}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  placeholder="Пароль"
                  className="w-full pl-11 pr-11 py-3 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary placeholder:text-text-muted dark:placeholder:text-text-dark-muted text-base outline-none focus:bg-white dark:focus:bg-surface-dark focus:border-primary-500 dark:focus:border-primary-400"
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute right-3.5 top-1/2 -translate-y-1/2 text-text-muted dark:text-text-dark-muted hover:text-text-secondary dark:hover:text-text-dark-secondary"
                >
                  {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full py-3 rounded-lg font-medium bg-primary-600 dark:bg-primary-500 text-white hover:bg-primary-700 dark:hover:bg-primary-600 disabled:opacity-50 disabled:cursor-not-allowed text-base"
            >
              {loading ? "Вход..." : "Войти"}
            </button>
          </form>
        </div>

        <p className="text-center text-sm text-text-muted dark:text-text-dark-muted mt-6">
          Аккаунты создаёт администратор
        </p>
      </div>
    </div>
  );
}
