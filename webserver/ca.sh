#https://letsencrypt.org/zh-cn/docs/certificates-for-localhost/
openssl req -x509 -out localhost.crt -keyout localhost.key -days 3650 \
    -newkey rsa:2048 -nodes -sha256 \
    -subj '/CN=test.lan' -extensions EXT -config <(
        printf "[dn]\nCN=test.lan\n[req]\ndistinguished_name = dn\n[EXT]\nsubjectAltName=DNS:test.lan\nkeyUsage=digitalSignature\nextendedKeyUsage=serverAuth"
    )
