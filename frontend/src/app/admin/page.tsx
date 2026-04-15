"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { Users, Folder, UserPlus, FolderPlus, Trash2, X, Plus, Shield, GraduationCap, User } from "lucide-react";
import ThemeToggle from "../components/ThemeToggle";
import * as jose from "jose";

const API = "http://localhost:5280/api";

type UserRole = "Student" | "Teacher" | "Admin";

interface User {
  id: number;
  fullName: string;
  login: string;
  role: UserRole;
  groupId?: number;
}

interface Group {
  id: number;
  groupName: string;
}

interface GroupWithUsers {
  id: number;
  name: string;
  users: User[];
}

type TabKey = "users" | "groups";
type RoleFilter = "all" | UserRole;

const ROLE_LABELS: Record<UserRole, string> = {
  Admin: "Админ",
  Teacher: "Преподаватель",
  Student: "Ученик",
};

const ROLE_ICONS: Record<UserRole, React.ReactNode> = {
  Admin: <Shield size={14} />,
  Teacher: <GraduationCap size={14} />,
  Student: <User size={14} />,
};

const ROLE_COLORS: Record<UserRole, string> = {
  Admin: "text-red-600 dark:text-red-400",
  Teacher: "text-primary-600 dark:text-primary-400",
  Student: "text-accent-600 dark:text-accent-400",
};

export default function AdminPage() {
  const router = useRouter();
  const [users, setUsers] = useState<User[]>([]);
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [activeTab, setActiveTab] = useState<TabKey>("users");
  const [roleFilter, setRoleFilter] = useState<RoleFilter>("all");

  const [showCreateUser, setShowCreateUser] = useState(false);
  const [showCreateGroup, setShowCreateGroup] = useState(false);
  const [selectedGroup, setSelectedGroup] = useState<GroupWithUsers | null>(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      router.push("/login");
      return;
    }

    const payload = jose.decodeJwt(token);
    if (payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] !== "Admin") {
      router.push("/");
      return;
    }

    loadData();
  }, []);

  const loadData = async () => {
    const token = localStorage.getItem("token");
    if (!token) return;

    try {
      const [usersRes, groupsRes] = await Promise.all([
        fetch(`${API}/user`, { headers: { Authorization: `Bearer ${token}` } }),
        fetch(`${API}/admin/groups`, { headers: { Authorization: `Bearer ${token}` } }),
      ]);

      if (usersRes.ok) {
        const data = await usersRes.json();
        setUsers(Array.isArray(data) ? data : []);
      }
      if (groupsRes.ok) {
        const data = await groupsRes.json();
        setGroups(Array.isArray(data) ? data : []);
      }
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    router.push("/login");
  };

  const filteredUsers = roleFilter === "all" ? users : users.filter((u) => u.role === roleFilter);

  return (
    <div className="min-h-screen bg-primary-50 dark:bg-background-dark">
      {/* Шапка */}
      <header className="bg-white dark:bg-surface-dark border-b border-border-light dark:border-border-dark px-6 py-4 flex items-center justify-between">
        <div className="flex items-center gap-3">
          <div className="w-9 h-9 rounded-full bg-primary-600 dark:bg-primary-500 flex items-center justify-center">
            <span className="text-sm font-bold text-white">J</span>
          </div>
          <h1 className="text-lg font-semibold text-text-primary dark:text-text-dark-primary">
            Admin Panel
          </h1>
        </div>
        <div className="flex items-center gap-2">
          <ThemeToggle />
          <button
            onClick={handleLogout}
            className="px-3 py-1.5 text-sm rounded-lg border border-border-light dark:border-border-dark text-text-secondary dark:text-text-dark-secondary hover:bg-gray-50 dark:hover:bg-gray-800"
          >
            Выйти
          </button>
        </div>
      </header>

      {/* Контент */}
      <main className="max-w-4xl mx-auto px-6 py-8">
        {error && (
          <div className="mb-6 p-3 text-sm text-red-700 bg-red-50 dark:bg-red-950/30 border border-red-200 dark:border-red-900 rounded-lg">
            {error}
          </div>
        )}

        {/* Главные вкладки */}
        <div className="flex gap-1 bg-gray-100 dark:bg-gray-800 rounded-lg p-1 mb-6">
          <button
            onClick={() => { setActiveTab("users"); setRoleFilter("all"); }}
            className={`flex-1 flex items-center justify-center gap-2 py-2 text-sm font-medium rounded-md transition ${
              activeTab === "users"
                ? "bg-white dark:bg-surface-dark text-text-primary dark:text-text-dark-primary shadow-sm"
                : "text-text-muted dark:text-text-dark-muted"
            }`}
          >
            <Users size={16} />
            Пользователи
          </button>
          <button
            onClick={() => setActiveTab("groups")}
            className={`flex-1 flex items-center justify-center gap-2 py-2 text-sm font-medium rounded-md transition ${
              activeTab === "groups"
                ? "bg-white dark:bg-surface-dark text-text-primary dark:text-text-dark-primary shadow-sm"
                : "text-text-muted dark:text-text-dark-muted"
            }`}
          >
            <Folder size={16} />
            Группы
          </button>
        </div>

        {/* ── Вкладка Пользователи ── */}
        {activeTab === "users" && (
          <div>
            {/* Фильтр по ролям + кнопка */}
            <div className="flex items-center justify-between mb-4">
              <div className="flex gap-1 bg-gray-100 dark:bg-gray-800 rounded-lg p-1">
                {(["all", "Admin", "Teacher", "Student"] as RoleFilter[]).map((role) => (
                  <button
                    key={role}
                    onClick={() => setRoleFilter(role)}
                    className={`px-3 py-1.5 text-xs font-medium rounded-md transition ${
                      roleFilter === role
                        ? "bg-white dark:bg-surface-dark text-text-primary dark:text-text-dark-primary shadow-sm"
                        : "text-text-muted dark:text-text-dark-muted"
                    }`}
                  >
                    {role === "all" ? "Все" : ROLE_LABELS[role]}
                  </button>
                ))}
              </div>
              <button
                onClick={() => setShowCreateUser(true)}
                className="p-2 rounded-lg bg-primary-600 dark:bg-primary-500 text-white hover:bg-primary-700 dark:hover:bg-primary-600"
              >
                <UserPlus size={16} />
              </button>
            </div>

            {/* Список */}
            <div className="bg-white dark:bg-surface-dark rounded-xl border border-border-light dark:border-border-dark divide-y divide-border-light dark:divide-border-dark">
              {loading ? (
                <div className="px-5 py-8 text-center text-text-muted dark:text-text-dark-muted">
                  Загрузка...
                </div>
              ) : filteredUsers.length === 0 ? (
                <div className="px-5 py-8 text-center text-text-muted dark:text-text-dark-muted">
                  Нет пользователей
                </div>
              ) : (
                filteredUsers.map((user) => (
                  <div key={user.id} className="flex items-center justify-between px-5 py-3">
                    <div className="flex items-center gap-3">
                      <div className={`p-1.5 rounded-lg bg-gray-50 dark:bg-gray-800 ${ROLE_COLORS[user.role]}`}>
                        {ROLE_ICONS[user.role]}
                      </div>
                      <div>
                        <p className="text-sm font-medium text-text-primary dark:text-text-dark-primary">
                          {user.fullName || user.login}
                        </p>
                        <p className="text-xs text-text-muted dark:text-text-dark-muted">
                          {user.login}
                        </p>
                      </div>
                    </div>
                    <button
                      onClick={() => {
                        if (confirm(`Удалить ${user.fullName || user.login}?`)) {
                          handleDeleteUser(user.id);
                        }
                      }}
                      className="p-1.5 rounded-lg hover:bg-red-50 dark:hover:bg-red-950/30 text-text-muted dark:text-text-dark-muted hover:text-red-600"
                    >
                      <Trash2 size={14} />
                    </button>
                  </div>
                ))
              )}
            </div>

            {/* Счётчик */}
            <p className="text-xs text-text-muted dark:text-text-dark-muted mt-3">
              Показано {filteredUsers.length} из {users.length}
            </p>
          </div>
        )}

        {/* ── Вкладка Группы ── */}
        {activeTab === "groups" && (
          <div>
            <div className="flex items-center justify-between mb-4">
              <p className="text-sm text-text-muted dark:text-text-dark-muted">
                Всего групп: {groups.length}
              </p>
              <button
                onClick={() => setShowCreateGroup(true)}
                className="p-2 rounded-lg bg-accent-500 dark:bg-accent-400 text-white hover:bg-accent-600 dark:hover:bg-accent-500"
              >
                <FolderPlus size={16} />
              </button>
            </div>

            <div className="bg-white dark:bg-surface-dark rounded-xl border border-border-light dark:border-border-dark divide-y divide-border-light dark:divide-border-dark">
              {loading ? (
                <div className="px-5 py-8 text-center text-text-muted dark:text-text-dark-muted">
                  Загрузка...
                </div>
              ) : groups.length === 0 ? (
                <div className="px-5 py-8 text-center text-text-muted dark:text-text-dark-muted">
                  Нет групп
                </div>
              ) : (
                groups.map((group) => (
                  <div key={group.id} className="flex items-center justify-between px-5 py-3">
                    <button
                      onClick={() => loadGroup(group.id)}
                      className="text-sm font-medium text-text-primary dark:text-text-dark-primary hover:text-primary-600 dark:hover:text-primary-400"
                    >
                      {group.groupName}
                    </button>
                    <button
                      onClick={() => {
                        if (confirm(`Удалить группу ${group.groupName}?`)) {
                          handleDeleteGroup(group.id);
                        }
                      }}
                      className="p-1.5 rounded-lg hover:bg-red-50 dark:hover:bg-red-950/30 text-text-muted dark:text-text-dark-muted hover:text-red-600"
                    >
                      <Trash2 size={14} />
                    </button>
                  </div>
                ))
              )}
            </div>
          </div>
        )}
      </main>

      {/* Модалки */}
      {showCreateUser && (
        <CreateUserModal
          onClose={() => setShowCreateUser(false)}
          onSuccess={() => { setShowCreateUser(false); loadData(); }}
          groups={groups}
        />
      )}
      {showCreateGroup && (
        <CreateGroupModal
          onClose={() => setShowCreateGroup(false)}
          onSuccess={() => { setShowCreateGroup(false); loadData(); }}
        />
      )}
      {selectedGroup && (
        <GroupViewModal
          group={selectedGroup}
          onClose={() => setSelectedGroup(null)}
          onSuccess={() => { setSelectedGroup(null); loadData(); }}
          groups={groups}
          allUsers={users}
        />
      )}
    </div>
  );

  async function handleDeleteUser(id: number) {
    const token = localStorage.getItem("token");
    if (!token) return;
    try {
      const res = await fetch(`${API}/user/${id}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
      });
      if (res.ok) loadData();
      else setError("Ошибка при удалении");
    } catch {
      setError("Ошибка при удалении");
    }
  }

  async function handleDeleteGroup(id: number) {
    const token = localStorage.getItem("token");
    if (!token) return;
    try {
      const res = await fetch(`${API}/admin/groups?id=${id}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
      });
      if (res.ok) loadData();
      else setError("Ошибка при удалении группы");
    } catch {
      setError("Ошибка при удалении группы");
    }
  }

  async function loadGroup(id: number) {
    const token = localStorage.getItem("token");
    if (!token) return;
    try {
      const res = await fetch(`${API}/admin/groups/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (res.ok) {
        const data = await res.json();
        setSelectedGroup(data);
      }
    } catch {
      setError("Ошибка при загрузке группы");
    }
  }
}

/* ─── CreateUser Modal ─── */

function CreateUserModal({ onClose, onSuccess, groups }: {
  onClose: () => void; onSuccess: () => void; groups: Group[];
}) {
  const [fullName, setFullName] = useState("");
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState<UserRole>("Student");
  const [groupId, setGroupId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");
    const token = localStorage.getItem("token");
    if (!token) return;
    try {
      const res = await fetch(`${API}/user`, {
        method: "POST",
        headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
        body: JSON.stringify({
          fullName, login, passwordHash: password,
          role: role === "Student" ? 0 : role === "Teacher" ? 1 : 2,
          groupId,
        }),
      });
      if (!res.ok) throw new Error("Ошибка при создании пользователя");
      onSuccess();
    } catch (err: any) { setError(err.message); }
    finally { setLoading(false); }
  };

  return (
    <Modal onClose={onClose}>
      <h2 className="text-lg font-semibold text-text-primary dark:text-text-dark-primary mb-4">
        Новый пользователь
      </h2>
      {error && (
        <div className="mb-4 text-sm text-red-700 bg-red-50 dark:bg-red-950/30 border border-red-200 dark:border-red-900 p-3 rounded-lg">
          {error}
        </div>
      )}
      <form onSubmit={handleSubmit} className="space-y-3">
        <input type="text" value={fullName} onChange={(e) => setFullName(e.target.value)} placeholder="ФИО"
          className="w-full px-3 py-2.5 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary placeholder:text-text-muted text-sm outline-none focus:border-primary-500" />
        <input type="text" value={login} onChange={(e) => setLogin(e.target.value)} placeholder="Логин"
          className="w-full px-3 py-2.5 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary placeholder:text-text-muted text-sm outline-none focus:border-primary-500" />
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Пароль"
          className="w-full px-3 py-2.5 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary placeholder:text-text-muted text-sm outline-none focus:border-primary-500" />
        <select value={role} onChange={(e) => setRole(e.target.value as UserRole)}
          className="w-full px-3 py-2.5 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary text-sm outline-none focus:border-primary-500">
          <option value="Student">Ученик</option>
          <option value="Teacher">Преподаватель</option>
          <option value="Admin">Админ</option>
        </select>
        {groups.length > 0 && (
          <select value={groupId ?? ""} onChange={(e) => setGroupId(e.target.value ? Number(e.target.value) : null)}
            className="w-full px-3 py-2.5 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary text-sm outline-none focus:border-primary-500">
            <option value="">Без группы</option>
            {groups.map((g) => (
              <option key={g.id} value={g.id}>{g.groupName}</option>
            ))}
          </select>
        )}
        <div className="flex gap-2 pt-2">
          <button type="button" onClick={onClose}
            className="flex-1 py-2.5 rounded-lg border border-border-light dark:border-border-dark text-text-secondary dark:text-text-dark-secondary text-sm hover:bg-gray-50 dark:hover:bg-gray-800">
            Отмена
          </button>
          <button type="submit" disabled={loading}
            className="flex-1 py-2.5 rounded-lg bg-primary-600 dark:bg-primary-500 text-white text-sm font-medium hover:bg-primary-700 disabled:opacity-50">
            {loading ? "Создание..." : "Создать"}
          </button>
        </div>
      </form>
    </Modal>
  );
}

/* ─── CreateGroup Modal ─── */

function CreateGroupModal({ onClose, onSuccess }: {
  onClose: () => void; onSuccess: () => void;
}) {
  const [name, setName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");
    const token = localStorage.getItem("token");
    if (!token) return;
    try {
      const res = await fetch(`${API}/admin/groups`, {
        method: "POST",
        headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
        body: JSON.stringify({ groupName: name }),
      });
      if (!res.ok) throw new Error("Ошибка при создании группы");
      onSuccess();
    } catch (err: any) { setError(err.message); }
    finally { setLoading(false); }
  };

  return (
    <Modal onClose={onClose}>
      <h2 className="text-lg font-semibold text-text-primary dark:text-text-dark-primary mb-4">
        Новая группа
      </h2>
      {error && (
        <div className="mb-4 text-sm text-red-700 bg-red-50 dark:bg-red-950/30 border border-red-200 dark:border-red-900 p-3 rounded-lg">
          {error}
        </div>
      )}
      <form onSubmit={handleSubmit} className="space-y-3">
        <input type="text" value={name} onChange={(e) => setName(e.target.value)} placeholder="Название группы"
          className="w-full px-3 py-2.5 rounded-lg bg-gray-50 dark:bg-background-dark border border-border-light dark:border-border-dark text-text-primary dark:text-text-dark-primary placeholder:text-text-muted text-sm outline-none focus:border-primary-500" />
        <div className="flex gap-2 pt-2">
          <button type="button" onClick={onClose}
            className="flex-1 py-2.5 rounded-lg border border-border-light dark:border-border-dark text-text-secondary dark:text-text-dark-secondary text-sm hover:bg-gray-50 dark:hover:bg-gray-800">
            Отмена
          </button>
          <button type="submit" disabled={loading}
            className="flex-1 py-2.5 rounded-lg bg-accent-500 dark:bg-accent-400 text-white text-sm font-medium hover:bg-accent-600 disabled:opacity-50">
            {loading ? "Создание..." : "Создать"}
          </button>
        </div>
      </form>
    </Modal>
  );
}

/* ─── Group View Modal ─── */

function GroupViewModal({ group, onClose, onSuccess, allUsers }: {
  group: GroupWithUsers; onClose: () => void; onSuccess: () => void;
  groups: Group[]; allUsers: User[];
}) {
  const [error, setError] = useState("");

  const handleAddUser = async (userId: number) => {
    const token = localStorage.getItem("token");
    if (!token) return;
    try {
      const res = await fetch(`${API}/admin/groups/${group.id}/${userId}`, {
        method: "PATCH", headers: { Authorization: `Bearer ${token}` },
      });
      if (res.ok) onSuccess(); else setError("Ошибка при добавлении");
    } catch { setError("Ошибка при добавлении"); }
  };

  const handleRemoveUser = async (userId: number) => {
    const token = localStorage.getItem("token");
    if (!token) return;
    try {
      const res = await fetch(`${API}/admin/groups/${group.id}/remove/${userId}`, {
        method: "PATCH", headers: { Authorization: `Bearer ${token}` },
      });
      if (res.ok) onSuccess(); else setError("Ошибка при удалении");
    } catch { setError("Ошибка при удалении"); }
  };

  const usersWithoutGroup = allUsers.filter((u) => !u.groupId);

  return (
    <Modal onClose={onClose}>
      <h2 className="text-lg font-semibold text-text-primary dark:text-text-dark-primary mb-1">
        {group.name}
      </h2>
      <p className="text-sm text-text-muted dark:text-text-dark-muted mb-4">
        {group.users.length} участников
      </p>
      {error && (
        <div className="mb-4 text-sm text-red-700 bg-red-50 dark:bg-red-950/30 border border-red-200 dark:border-red-900 p-3 rounded-lg">
          {error}
        </div>
      )}
      <div className="mb-4 space-y-2 max-h-48 overflow-y-auto">
        {group.users.length === 0 ? (
          <p className="text-sm text-text-muted dark:text-text-dark-muted text-center py-4">Нет участников</p>
        ) : (
          group.users.map((user) => (
            <div key={user.id} className="flex items-center justify-between bg-gray-50 dark:bg-background-dark rounded-lg px-3 py-2">
              <div>
                <p className="text-sm font-medium text-text-primary dark:text-text-dark-primary">
                  {user.fullName || user.login}
                </p>
                <p className="text-xs text-text-muted dark:text-text-dark-muted">{user.login}</p>
              </div>
              <button onClick={() => handleRemoveUser(user.id)}
                className="p-1 rounded hover:bg-red-100 dark:hover:bg-red-950/30 text-text-muted hover:text-red-600">
                <X size={14} />
              </button>
            </div>
          ))
        )}
      </div>
      {usersWithoutGroup.length > 0 && (
        <div>
          <p className="text-sm font-medium text-text-secondary dark:text-text-dark-secondary mb-2">
            Добавить пользователя
          </p>
          <div className="space-y-1 max-h-32 overflow-y-auto">
            {usersWithoutGroup.map((user) => (
              <button key={user.id} onClick={() => handleAddUser(user.id)}
                className="w-full flex items-center justify-between bg-gray-50 dark:bg-background-dark rounded-lg px-3 py-2 hover:bg-primary-50 dark:hover:bg-primary-900/20">
                <span className="text-sm text-text-primary dark:text-text-dark-primary">
                  {user.fullName || user.login}
                </span>
                <Plus size={14} className="text-text-muted" />
              </button>
            ))}
          </div>
        </div>
      )}
    </Modal>
  );
}

/* ─── Modal ─── */

function Modal({ children, onClose }: { children: React.ReactNode; onClose: () => void }) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4" onClick={onClose}>
      <div className="bg-white dark:bg-surface-dark rounded-xl border border-border-light dark:border-border-dark p-6 w-full max-w-md shadow-xl" onClick={(e) => e.stopPropagation()}>
        {children}
      </div>
    </div>
  );
}
