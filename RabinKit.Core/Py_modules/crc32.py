import zlib

def calculate_crc32(data):
    return zlib.crc32(data.encode('utf-8')) & 0xffffffff