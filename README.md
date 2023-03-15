# 轻量级微服务架构
### 技术架构组成 
- Sqlsugar ORM框架 连接 MySql
- Consul 服务注册与发现
- MVC客户端 通过 Consul 调用API服务
- Ocelot API网关继承Consul
- 使用Scrutor 注入工具批量注入仓储服务
- 持续更新中......
![image](https://user-images.githubusercontent.com/23447209/225196260-91f0a140-006f-4e39-aec2-14ff47f72199.png)
