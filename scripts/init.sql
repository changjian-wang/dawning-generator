-- Dawning Generator 数据库初始化脚本
-- 创建时间: 2025-12-25

CREATE DATABASE IF NOT EXISTS dawning_generator CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE dawning_generator;

-- 项目生成历史
CREATE TABLE IF NOT EXISTS project_histories (
    id CHAR(36) PRIMARY KEY,
    user_id CHAR(36) NOT NULL,
    project_name VARCHAR(100) NOT NULL,
    namespace_prefix VARCHAR(100),
    architecture_type VARCHAR(20) NOT NULL,      -- 'layered' | 'simple'
    dotnet_version VARCHAR(10) NOT NULL,         -- 'net8.0' | 'net9.0'
    database_type VARCHAR(20) NOT NULL,          -- 'mysql' | 'postgresql' | 'sqlserver' | 'sqlite'
    selected_modules JSON NOT NULL,              -- ["identity", "core", "logging", ...]
    optional_features JSON NOT NULL,             -- ["docker", "tests", "helm", "github-actions", ...]
    service_port INT NOT NULL,
    download_count INT DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_user_id (user_id),
    INDEX idx_created_at (created_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 模板收藏
CREATE TABLE IF NOT EXISTS template_favorites (
    id CHAR(36) PRIMARY KEY,
    user_id CHAR(36) NOT NULL,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    config JSON NOT NULL,                        -- 完整配置快照
    is_default BOOLEAN DEFAULT FALSE,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_user_id (user_id),
    UNIQUE KEY uk_user_name (user_id, name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 生成统计 (管理员用)
CREATE TABLE IF NOT EXISTS generation_stats (
    id CHAR(36) PRIMARY KEY,
    date DATE NOT NULL,
    total_generations INT DEFAULT 0,
    unique_users INT DEFAULT 0,
    by_architecture JSON,                        -- {"layered": 100, "simple": 50}
    by_database JSON,                            -- {"mysql": 80, "postgresql": 40, ...}
    by_module JSON,                              -- {"identity": 150, "caching": 80, ...}
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uk_date (date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
