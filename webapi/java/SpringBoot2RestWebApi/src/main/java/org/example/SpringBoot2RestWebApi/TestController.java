package org.example.SpringBoot2RestWebApi;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpServletRequest;
import java.util.Collections;
import java.util.HashMap;

@RestController
public class TestController {
    private @Autowired
    HttpServletRequest request;

    @GetMapping("/test")
    public ResponseEntity<?> greeting() {
        return ResponseEntity.ok(new HashMap<String, Object>() {
            {
                put("Version", "0.0.3");
                put("HeaderNames", String.join(";", Collections.list(request.getHeaderNames())));
                put("Host", request.getRemoteHost());
            }
        });
    }
}