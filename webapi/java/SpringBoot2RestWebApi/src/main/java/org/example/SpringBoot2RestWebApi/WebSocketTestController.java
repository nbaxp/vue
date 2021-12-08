package org.example.SpringBoot2RestWebApi;


import org.springframework.stereotype.Component;

import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;
import java.io.IOException;

@ServerEndpoint("/ws")
@Component
public class WebSocketTestController {

    @OnOpen
    public void onOpen(Session session) {
        System.out.println("连接成功");
    }


    @OnClose
    public void onClose(Session session) {
        System.out.println("连接关闭");
    }

    @OnMessage
    public String onMsg(String text) throws IOException {
        return "servet 发送：" + text;
    }
}
